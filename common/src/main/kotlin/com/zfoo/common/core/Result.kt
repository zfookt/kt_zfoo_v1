package com.zfoo.common.core

data class Result<T>(
    val code: Int,
    val message: String,
    val data: T? = null
)
