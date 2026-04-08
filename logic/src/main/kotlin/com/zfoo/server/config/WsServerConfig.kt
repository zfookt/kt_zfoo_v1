package com.zfoo.server.config

import com.zfoo.server.netty.WsServer
import org.springframework.beans.factory.annotation.Value
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration

@Configuration
@ConditionalOnProperty(name = ["app"], havingValue = "logic")
class WsServerConfig {

    @Value("\${ws.server.port}")
    private var port: Int = 0

    @Bean
    fun wsServer(): WsServer {
        return WsServer(port)
    }
}
