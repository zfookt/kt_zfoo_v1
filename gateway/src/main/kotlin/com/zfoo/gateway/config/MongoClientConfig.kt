package com.zfoo.gateway.config

import com.mongodb.client.MongoClient
import com.mongodb.client.MongoClients
import org.slf4j.LoggerFactory
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.beans.factory.annotation.Value
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.data.mongodb.core.MongoTemplate

@Configuration
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
class MongoClientConfig {
    private val log = LoggerFactory.getLogger(javaClass)

    @Value("\${app.mongo.uri:mongodb://127.0.0.1:37017/kt_zfoo}")
    private lateinit var mongoUri: String

    @Value("\${app.mongo.database:kt_zfoo}")
    private lateinit var database: String

    @Bean(destroyMethod = "close")
    fun mongoClient(): MongoClient {
        log.info("mongo client init with uri={}", mongoUri)
        return MongoClients.create(mongoUri)
    }

    @Bean
    fun mongoTemplate(): MongoTemplate {
        log.info("mongo template init, database={}", database)
        return MongoTemplate(mongoClient(), database)
    }
}
