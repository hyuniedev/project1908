import com.android.build.api.dsl.Packaging

plugins {
    id("com.android.library")
    id("org.jetbrains.kotlin.android")
}

android {
    namespace = "com.hyunie.stt_tts_plugin"
    compileSdk = 35

    defaultConfig {
        minSdk = 24
        targetSdk = 34

        // Cho phép Proguard rules khi Unity build
        consumerProguardFiles("consumer-rules.pro")
    }

    buildTypes {
        release {
            // Tắt minify để tránh rối code
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
        debug {
            isMinifyEnabled = false
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
    kotlinOptions {
        jvmTarget = "1.8"
    }

    packagingOptions {
        resources {
            excludes += "META-INF/DEPENDENCIES"
            excludes += "META-INF/LICENSE"
            excludes += "META-INF/LICENSE.txt"
            excludes += "META-INF/NOTICE"
            excludes += "META-INF/NOTICE.txt"
        }
    }
}

dependencies {
    // Chỉ dùng UnityPlayer.jar cho compile, không đóng gói vào .aar
    compileOnly(files("libs/UnityPlayer.jar"))

    // Thư viện Android cơ bản
    implementation("androidx.core:core-ktx:1.12.0")
    implementation("androidx.appcompat:appcompat:1.7.0")
    implementation("com.google.android.material:material:1.11.0")
}