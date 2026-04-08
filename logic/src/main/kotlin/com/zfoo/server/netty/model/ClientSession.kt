package com.zfoo.server.netty.model

import io.netty.channel.Channel
import org.slf4j.LoggerFactory

class ClientSession(private val channel: Channel) {
    private val log = LoggerFactory.getLogger(javaClass)

    val connId: String = channel.id().asLongText()
    val userId: Long = 0L

    fun disconnect() {
        if (channel.isActive) {
            channel.close()
        }
    }

    fun parseAndDispatchMessage(message: ProtobufMessage) {
        // 这里保留和 Java 版同样的入口，后续可接入真实 protobuf 分发
        log.info("receive ws message, connId={}, msgId={}, bodySize={}", connId, message.msgId, message.body.size)
    }
}
