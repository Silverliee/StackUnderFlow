package com.example.stackunderflow.module

import com.example.stackunderflow.repository.ScriptRepository
import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.service.StackUnderFlowApiService
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import com.example.stackunderflow.utils.AuthInterceptor
import com.example.stackunderflow.utils.Constants
import com.example.stackunderflow.utils.SessionManager
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.core.qualifier.named
import org.koin.dsl.module

// Module that provides core dependencies for the application,
// such as the repositories, the view models and the services which are are singletons,
// so they will be created only once and will be reused
internal val coreModules = module {
    single { UsersRepository(get(named("apiStackUnderFlow"))) }

    single { SessionManager(get()) }

    single { AuthInterceptor(get()) }

    viewModel { UserViewModel(get(),get()) }
    single { ScriptRepository(get(named("apiStackUnderFlow"))) }  // Correction ici

    viewModel { ScriptViewModel(get()) }

    single(named("apiStackUnderFlow")) {
        createWebService<StackUnderFlowApiService>(get(named(Constants.apiStackUnderFlow)))
    }
}

