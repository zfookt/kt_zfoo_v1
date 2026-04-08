package com.zfoo.gateway.controller

import com.zfoo.common.core.Result
import com.zfoo.common.dto.LoginResponse
import com.zfoo.gateway.controller.dto.LoginRequest
import com.zfoo.gateway.service.AccountService
import jakarta.validation.Valid
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.web.bind.annotation.PostMapping
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@ConditionalOnProperty(name = ["app"], havingValue = "gateway")
@RequestMapping("/account")
class AccountController {

    @Autowired
    private lateinit var accountService: AccountService

    @PostMapping("/login")
    fun login(@Valid @RequestBody request: LoginRequest): Result<LoginResponse> {
        return accountService.login(request.openId.trim())
    }
}
