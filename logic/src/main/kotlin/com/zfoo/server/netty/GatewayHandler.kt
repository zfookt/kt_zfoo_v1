package com.zfoo.server.netty

import com.zfoo.server.netty.model.ClientSession
import com.zfoo.server.netty.model.ProtobufMessage
import io.netty.channel.ChannelHandlerContext
import io.netty.channel.ChannelInboundHandlerAdapter
import io.netty.handler.timeout.IdleState
import io.netty.handler.timeout.IdleStateEvent
import io.netty.util.ReferenceCountUtil
import org.slf4j.LoggerFactory

// 一玩家对应一个
class GatewayHandler : ChannelInboundHandlerAdapter() {
    private val log = LoggerFactory.getLogger(javaClass)

    private var session: ClientSession? = null

    // 连上网络
    override fun channelActive(ctx: ChannelHandlerContext) {
        session = ClientSession(ctx.channel())
    }

    // 断开
    override fun channelInactive(ctx: ChannelHandlerContext) {
        session?.disconnect()
    }

    // 收到数据
    override fun channelRead(ctx: ChannelHandlerContext, msg: Any?) {
        try {
            if (msg == null) {
                return
            }

            val message = msg as? ProtobufMessage ?: return
            session?.parseAndDispatchMessage(message)
        } catch (e: Exception) {
            log.error("gateway read error", e)
        } finally {
            ReferenceCountUtil.release(msg)
        }
    }

    // 空闲检测
    override fun userEventTriggered(ctx: ChannelHandlerContext, evt: Any) {
        if (evt is IdleStateEvent) {
            when (evt.state()) {
                IdleState.READER_IDLE -> {
                    log.error("reader idle timeout, disconnect, userId={}, connId={}", session?.userId, session?.connId)
                    ctx.disconnect()
                }

                IdleState.WRITER_IDLE -> {
                    log.error("writer idle timeout, disconnect, userId={}, connId={}", session?.userId, session?.connId)
                    ctx.disconnect()
                }

                else -> Unit
            }
        }
    }

    // 异常
    override fun exceptionCaught(ctx: ChannelHandlerContext, cause: Throwable) {
        val connId = session?.connId
        val message = cause.message.orEmpty()
        if (message.contains("Connection reset by peer")) {
            log.error("connection reset by peer, connId={}", connId)
        } else {
            log.error("channel exception, connId={}", connId, cause)
        }
    }

    // 读写状态发生改变
    override fun channelWritabilityChanged(ctx: ChannelHandlerContext) {
        super.channelWritabilityChanged(ctx)
        log.error("channel writability changed, userId={}, connId={}", session?.userId, session?.connId)
    }
}