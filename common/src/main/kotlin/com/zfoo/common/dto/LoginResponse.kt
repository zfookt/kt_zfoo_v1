package com.zfoo.common.dto

data class LoginResponse(
    val token: String,
    val serverInfoList: List<ServerInfo>
)
