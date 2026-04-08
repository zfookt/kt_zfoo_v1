package com.zfoo.server

import com.zfoo.launcher.LauncherApplication
import org.junit.jupiter.api.Test
import org.springframework.boot.test.context.SpringBootTest

@SpringBootTest(
    classes = [LauncherApplication::class],
    properties = ["app=logic", "spring.config.name=logic", "spring.main.web-application-type=none"]
)
class ServerApplicationTests {

    @Test
    fun contextLoads() {
    }

}
