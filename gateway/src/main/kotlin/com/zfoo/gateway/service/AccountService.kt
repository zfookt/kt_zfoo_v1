package com.zfoo.gateway.service

import com.zfoo.common.core.Result
import com.zfoo.common.core.ResultGenerator
import com.zfoo.common.dto.LoginResponse
import com.zfoo.common.dto.ServerInfo
import com.zfoo.gateway.config.GameServerProperties
import com.zfoo.gateway.mongo.entity.AccountDocument
import com.zfoo.gateway.mongo.entity.CounterDocument
import org.redisson.api.RedissonClient
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.dao.DuplicateKeyException
import org.springframework.data.mongodb.core.FindAndModifyOptions
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.stereotype.Service
import java.nio.charset.StandardCharsets
import java.time.Instant
import java.util.Base64
import java.util.UUID
import java.util.concurrent.TimeUnit

@Service
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
class AccountService(
    private val mongoTemplate: MongoTemplate,
    private val gameServerProperties: GameServerProperties,
    private val redissonClient: RedissonClient
) {
    fun login(openId: String): Result<LoginResponse> {
        val lock = redissonClient.getLock("account:openId:$openId")
        return try {
            lock.lock(5, TimeUnit.SECONDS)
            val account = findByOpenId(openId) ?: createAccount(openId)
            ResultGenerator.success(buildLoginResponse(account.userId))
        } finally {
            if (lock.isHeldByCurrentThread) {
                lock.unlock()
            }
        }
    }

    private fun findByOpenId(openId: String): AccountDocument? {
        val query = Query.query(Criteria.where("openId").`is`(openId))
        return mongoTemplate.findOne(query, AccountDocument::class.java, ACCOUNT_COLLECTION)
    }

    private fun createAccount(openId: String): AccountDocument {
        val now = Instant.now()
        val account = AccountDocument(
            openId = openId,
            userId = nextUserId(),
            createdAt = now,
            updatedAt = now
        )
        return try {
            mongoTemplate.save(account, ACCOUNT_COLLECTION)
        } catch (e: DuplicateKeyException) {
            findByOpenId(openId) ?: throw e
        }
    }

    private fun nextUserId(): Long {
        val query = Query.query(Criteria.where("_id").`is`(USER_ID_COUNTER_KEY))
        val update = Update()
            .inc("seq", 1)
            .setOnInsert("_id", USER_ID_COUNTER_KEY)
            .setOnInsert("seq", USER_ID_START)
        val options = FindAndModifyOptions.options().upsert(true).returnNew(true)
        val result = mongoTemplate.findAndModify(
            query,
            update,
            options,
            CounterDocument::class.java,
            COUNTER_COLLECTION
        )

        checkNotNull(result) { "failed to generate userId from counters collection" }
        return result.seq
    }

    private fun buildLoginResponse(userId: Long): LoginResponse {
        val token = buildToken(userId)
        val list = (1..gameServerProperties.count).map { index ->
            ServerInfo(
                name = "${gameServerProperties.namePrefix}$index",
                ip = gameServerProperties.ip,
                port = gameServerProperties.port
            )
        }
        return LoginResponse(token = token, serverInfoList = list)
    }

    private fun buildToken(userId: Long): String {
        val raw = "$userId:${System.currentTimeMillis()}:${UUID.randomUUID()}"
        return Base64.getUrlEncoder().withoutPadding().encodeToString(raw.toByteArray(StandardCharsets.UTF_8))
    }

    companion object {
        private const val ACCOUNT_COLLECTION = "account"
        private const val COUNTER_COLLECTION = "counters"
        private const val USER_ID_COUNTER_KEY = "user_id"
        private const val USER_ID_START = 100000L
    }
}
