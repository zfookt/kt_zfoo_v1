package com.zfoo.gateway.mongo.entity

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

@Document("counters")
data class CounterDocument(
    @Id
    val id: String,
    val seq: Long = 0L
)
