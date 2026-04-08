package com.zfoo.gateway.controller

import com.zfoo.common.core.Result
import com.zfoo.common.core.ResultGenerator
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.web.bind.MethodArgumentNotValidException
import org.springframework.web.bind.annotation.ExceptionHandler
import org.springframework.web.bind.annotation.RestControllerAdvice

@RestControllerAdvice
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
class GlobalExceptionHandler {

    @ExceptionHandler(MethodArgumentNotValidException::class)
    fun handleValidation(ex: MethodArgumentNotValidException): Result<Unit> {
        val message = ex.bindingResult.fieldErrors.firstOrNull()?.defaultMessage ?: "invalid request"
        return ResultGenerator.fail(message)
    }

    @ExceptionHandler(Exception::class)
    fun handleGeneric(ex: Exception): Result<Unit> {
        return ResultGenerator.fail(ex.message ?: "internal error")
    }
}
