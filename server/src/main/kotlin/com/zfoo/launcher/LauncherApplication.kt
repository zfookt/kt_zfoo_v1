package com.zfoo.launcher

import com.zfoo.common.start.ModuleStart
import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.beans.factory.annotation.Value
import org.springframework.boot.CommandLineRunner
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.context.ApplicationListener
import org.springframework.context.event.ContextClosedEvent

private const val MODE_ARG_PREFIX = "--app="
private const val MODE_GATEWAY = "gateway"
private const val MODE_LOGIC = "logic"

@SpringBootApplication(scanBasePackages = ["com.zfoo"])
class LauncherApplication : CommandLineRunner, ApplicationListener<ContextClosedEvent> {
    private val log = LoggerFactory.getLogger(javaClass)

    @Value("\${app:logic}")
    private lateinit var appMode: String

    @Autowired(required = false)
    private var moduleStarts: List<ModuleStart> = emptyList()

    private var currentStart: ModuleStart? = null

    override fun run(vararg args: String) {
        val mode = appMode.lowercase()
        if (mode != MODE_GATEWAY && mode != MODE_LOGIC) {
            throw IllegalStateException("Unsupported app mode '$mode'. Use app=gateway or app=logic.")
        }
        val selected = moduleStarts.firstOrNull { it.mode().equals(mode, ignoreCase = true) }
            ?: throw IllegalStateException("No ModuleStart found for app=$mode")

        currentStart = selected
        selected.start()
        log.info("launcher started mode={}", mode)
    }

    override fun onApplicationEvent(event: ContextClosedEvent) {
        currentStart?.stop()
    }
}

fun main(args: Array<String>) {
    val appMode = args.firstOrNull { it.startsWith(MODE_ARG_PREFIX) }
        ?.substringAfter(MODE_ARG_PREFIX)
        ?.lowercase()
        ?: throw IllegalArgumentException("Missing app mode. Use --app=gateway or --app=logic.")

    if (appMode != MODE_GATEWAY && appMode != MODE_LOGIC) {
        throw IllegalArgumentException("Unsupported app mode '$appMode'. Use --app=gateway or --app=logic.")
    }

    val bootArgs = mutableListOf<String>()
    bootArgs += args.toList()
    bootArgs += "--spring.config.name=${if (appMode == MODE_GATEWAY) "gateway" else "logic"}"
    if (appMode == MODE_LOGIC) {
        bootArgs += "--spring.main.web-application-type=none"
    }

    runApplication<LauncherApplication>(*bootArgs.distinct().toTypedArray())
}
