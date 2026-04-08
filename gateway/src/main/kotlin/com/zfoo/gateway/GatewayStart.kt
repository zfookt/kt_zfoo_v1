package com.zfoo.gateway

import com.zfoo.common.start.ModuleStart
import org.slf4j.LoggerFactory
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.stereotype.Component

@Component
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
class GatewayStart : ModuleStart {
    private val log = LoggerFactory.getLogger(javaClass)

    override fun mode(): String = "gateway"

    override fun start() {
        log.info("gateway start success")
    }

    override fun stop() {
        log.info("gateway stop success")
    }
}
