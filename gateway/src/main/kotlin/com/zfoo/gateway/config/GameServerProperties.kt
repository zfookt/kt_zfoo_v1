package com.zfoo.gateway.config

import org.springframework.boot.context.properties.ConfigurationProperties
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.stereotype.Component

@Component
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
@ConfigurationProperties(prefix = "game.server")
class GameServerProperties {
    var count: Int = 10
    var namePrefix: String = "server-"
    var ip: String = "127.0.0.1"
    var port: Int = 1111
}
