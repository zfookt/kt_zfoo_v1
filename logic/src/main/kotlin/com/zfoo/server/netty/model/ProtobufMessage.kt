package com.zfoo.server.netty.model

/**
 * WebSocket 二进制协议包: msgId(4 bytes) + body(bytes)
 */
data class ProtobufMessage(
    val msgId: Int,
    val body: ByteArray
)
