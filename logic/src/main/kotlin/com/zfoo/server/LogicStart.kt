package com.zfoo.server

import com.zfoo.common.start.ModuleStart
import com.zfoo.server.netty.WsServer
import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.stereotype.Component

@Component
@ConditionalOnProperty(name = ["app"], havingValue = "logic")
class LogicStart : ModuleStart {
    private val log = LoggerFactory.getLogger(javaClass)

    @Autowired
    private lateinit var wsServer: WsServer

    override fun mode(): String = "logic"

    override fun start() {
        wsServer.start()
        log.info("logic start success")
    }

    override fun stop() {
        log.warn("=====close logic start=====")
        wsServer.stop()
        log.warn("=====close logic end=====")
    }
}
