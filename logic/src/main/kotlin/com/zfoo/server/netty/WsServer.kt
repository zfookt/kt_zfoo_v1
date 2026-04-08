package com.zfoo.server.netty

import com.zfoo.server.netty.GatewayHandler
import com.zfoo.server.netty.codec.WsDecoder
import com.zfoo.server.netty.codec.WsEncoder
import io.netty.bootstrap.ServerBootstrap
import io.netty.buffer.PooledByteBufAllocator
import io.netty.channel.*
import io.netty.channel.nio.NioEventLoopGroup
import io.netty.channel.socket.SocketChannel
import io.netty.channel.socket.nio.NioServerSocketChannel
import io.netty.handler.codec.http.HttpObjectAggregator
import io.netty.handler.codec.http.HttpServerCodec
import io.netty.handler.codec.http.websocketx.WebSocketServerProtocolHandler
import io.netty.handler.timeout.IdleStateHandler
import org.slf4j.LoggerFactory


class WsServer(port: Int) {
    private val log = LoggerFactory.getLogger(javaClass)

    private val port = port

    private val bossGroup: EventLoopGroup = NioEventLoopGroup(1)
    private val workerGroup: EventLoopGroup = NioEventLoopGroup()

    fun start() {
        try {
            val bootstrap = ServerBootstrap()
            bootstrap.group(bossGroup, workerGroup)
                .channel(NioServerSocketChannel::class.java)
                .option(ChannelOption.SO_BACKLOG, 10240)
                .option(ChannelOption.ALLOCATOR, PooledByteBufAllocator.DEFAULT)
                .childOption(ChannelOption.ALLOCATOR, PooledByteBufAllocator.DEFAULT)
                .childOption(ChannelOption.TCP_NODELAY, true)
                .childOption(ChannelOption.SO_KEEPALIVE, true)
                .childOption(ChannelOption.WRITE_BUFFER_WATER_MARK, WriteBufferWaterMark(128 * 1024, 256 * 1024))
                .childHandler(object : ChannelInitializer<SocketChannel>() {
                    override fun initChannel(ch: SocketChannel) {
                        val p: ChannelPipeline = ch.pipeline()
                        val channelHandler: ChannelHandler = GatewayHandler()

                        p.addLast(IdleStateHandler(5 * 60, 5 * 60, 0))
                        p.addLast(HttpServerCodec())
                        p.addLast(HttpObjectAggregator(65535))
                        p.addLast(WebSocketServerProtocolHandler("/ws"))
                        p.addLast(WsDecoder())
                        p.addLast(WsEncoder())
                        p.addLast(channelHandler)

                    }
                })

            bootstrap.bind(port).sync().channel()

            log.info("WsServer started on port $port")
        } catch (e: Exception) {
            log.error(e.message, e)
            System.exit(1)
        }
    }

    fun stop() {
        bossGroup.shutdownGracefully()
        workerGroup.shutdownGracefully()
    }
}
