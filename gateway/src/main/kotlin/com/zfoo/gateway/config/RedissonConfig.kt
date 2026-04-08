package com.zfoo.gateway.config

import org.redisson.Redisson
import org.redisson.api.RedissonClient
import org.redisson.config.Config
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.beans.factory.annotation.Value
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration

@Configuration
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
class RedissonConfig {

    @Value("\${redisson.address}")
    private lateinit var address: String

    @Value("\${redisson.database:0}")
    private var database: Int = 0

    @Value("\${redisson.password:}")
    private var password: String = ""

    @Bean(destroyMethod = "shutdown")
    fun redissonClient(): RedissonClient {
        val config = Config()
        val singleServer = config.useSingleServer()
            .setAddress(address)
            .setDatabase(database)

        if (password.isNotBlank()) {
            singleServer.password = password
        }

        return Redisson.create(config)
    }
}
