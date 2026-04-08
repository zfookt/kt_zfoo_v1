package com.zfoo.gateway.mongo.entity

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.index.Indexed
import org.springframework.data.mongodb.core.mapping.Document
import java.time.Instant

@Document("account")
data class AccountDocument(
    @Id
    val id: String? = null,

    @Indexed(unique = true, name = "uk_account_open_id")
    val openId: String,

    @Indexed(unique = true, name = "uk_account_user_id")
    val userId: Long,

    val createdAt: Instant = Instant.now(),
    val updatedAt: Instant = Instant.now()
)
