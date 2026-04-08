package com.zfoo.server.netty.codec

import com.zfoo.server.netty.model.ProtobufMessage
import io.netty.channel.ChannelHandlerContext
import io.netty.channel.ChannelOutboundHandlerAdapter
import io.netty.channel.ChannelPromise
import io.netty.handler.codec.http.websocketx.BinaryWebSocketFrame

/**
 * WebSocket Binary Frame 编码:
 * msgId(4 bytes) + body(bytes)
 */
class WsEncoder : ChannelOutboundHandlerAdapter() {
    override fun write(ctx: ChannelHandlerContext, msg: Any, promise: ChannelPromise) {
        if (msg !is ProtobufMessage) {
            super.write(ctx, msg, promise)
            return
        }

        val byteBuf = ctx.alloc().buffer(Int.SIZE_BYTES + msg.body.size)
        byteBuf.writeInt(msg.msgId)
        byteBuf.writeBytes(msg.body)
        super.write(ctx, BinaryWebSocketFrame(byteBuf), promise)
    }
}
