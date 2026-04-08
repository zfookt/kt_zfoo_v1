package com.zfoo.common.core

object ResultGenerator {
    private const val DEFAULT_SUCCESS_MESSAGE = "SUCCESS"

    fun success(): Result<Unit> =
        Result(code = ResultCode.SUCCESS.code, message = DEFAULT_SUCCESS_MESSAGE)

    fun <T> success(data: T): Result<T> =
        Result(code = ResultCode.SUCCESS.code, message = DEFAULT_SUCCESS_MESSAGE, data = data)

    fun fail(message: String): Result<Unit> =
        Result(code = ResultCode.FAIL.code, message = message)
}
