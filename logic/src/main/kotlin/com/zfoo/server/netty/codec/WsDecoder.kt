package com.zfoo.server.netty.codec

import com.zfoo.server.netty.model.ProtobufMessage
import io.netty.channel.ChannelHandlerContext
import io.netty.channel.ChannelInboundHandlerAdapter
import io.netty.handler.codec.http.websocketx.BinaryWebSocketFrame
import org.slf4j.LoggerFactory

/**
 * WebSocket Binary Frame 解码:
 * msgId(4 bytes) + body(bytes)
 */
class WsDecoder : ChannelInboundHandlerAdapter() {
    private val log = LoggerFactory.getLogger(javaClass)

    override fun channelRead(ctx: ChannelHandlerContext, msg: Any) {
        if (msg !is BinaryWebSocketFrame) {
            ctx.fireChannelRead(msg)
            return
        }

        try {
            val byteBuf = msg.content()
            if (byteBuf.readableBytes() < Int.SIZE_BYTES) {
                log.error("invalid websocket frame, readableBytes={}", byteBuf.readableBytes())
                return
            }

            val msgId = byteBuf.readInt()
            val body = ByteArray(byteBuf.readableBytes())
            byteBuf.readBytes(body)

            ctx.fireChannelRead(ProtobufMessage(msgId, body))
        } finally {
            msg.release()
        }
    }
}
