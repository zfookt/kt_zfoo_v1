package com.zfoo.common.core

enum class ResultCode(val code: Int) {
    SUCCESS(200),
    FAIL(400),
    UNAUTHORIZED(401),
    NOT_FOUND(404),
    INTERNAL_SERVER_ERROR(500)
}
