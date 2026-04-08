package com.zfoo.gateway.controller.dto

import jakarta.validation.constraints.NotBlank

data class LoginRequest(
    @field:NotBlank(message = "openId is required")
    val openId: String
)
