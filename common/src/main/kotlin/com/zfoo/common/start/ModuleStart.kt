package com.zfoo.common.start

interface ModuleStart {
    fun mode(): String
    fun start()
    fun stop()
}
