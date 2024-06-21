package com.example.stackunderflow.module

import com.example.stackunderflow.utils.Constants
import com.google.gson.GsonBuilder
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import org.koin.core.qualifier.named
import org.koin.dsl.module
import retrofit2.Retrofit
import retrofit2.adapter.rxjava3.RxJava3CallAdapterFactory
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

internal val remoteModule = module {

    // Creation of the MusicBrainz API singleton (the main API that allows us to retrieve
    // information about artists, albums, songs, etc.)
    single(
        named(Constants.apiStackUnderFlow)
    ) {
        createRetrofit(get(named("apiStackUnderFlowHttpClient")), Constants.StackUnderFlowUrl)
    }
    single(named("apiStackUnderFlowHttpClient")) { createOkHttpClient() }

}


// Function to create an OkHttpClient instance with logging and User-Agent interceptors
private fun createOkHttpClient(): OkHttpClient {
    // Creating an interceptor to log HTTP requests and responses
    val interceptor = HttpLoggingInterceptor()
    interceptor.level = HttpLoggingInterceptor.Level.BODY

    // Creating an interceptor to add a User-Agent header to each request (required by the MusicBrainz API)
    val userAgentInterceptor = Interceptor { chain ->
        val request = chain.request().newBuilder()
            .header("User-Agent", Constants.userAgent)
            .build()
        chain.proceed(request)
    }
    // Creating and configuring an OkHttpClient client
    return OkHttpClient.Builder()
        .connectTimeout(20, TimeUnit.SECONDS)
        .readTimeout(20, TimeUnit.SECONDS)
        .addInterceptor(interceptor)
        .addInterceptor(userAgentInterceptor)
        .build()
}


// Function to create a Retrofit client
fun createRetrofit(okHttpClient: OkHttpClient, baseUrl: String): Retrofit {
    val gsonConverter =
        GsonConverterFactory.create(
            GsonBuilder().create()
        )
    // Configuring and building Retrofit
    return Retrofit.Builder()
        .baseUrl(baseUrl)
        .addConverterFactory(gsonConverter)
        .addCallAdapterFactory(RxJava3CallAdapterFactory.create())
        .client(okHttpClient)
        .build()
}

// Inline function to create a web service interface using Retrofit
inline fun <reified T> createWebService(retrofit: Retrofit): T {
    return retrofit.create(T::class.java)
}
