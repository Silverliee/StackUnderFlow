package com.example.stackunderflow.module

import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.service.StackUnderFlowApiService
import com.example.stackunderflow.utils.Constants
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.core.qualifier.named
import org.koin.dsl.module

// Module that provides core dependencies for the application,
// such as the repositories, the view models and the services which are are singletons,
// so they will be created only once and will be reused
internal val coreModules = module {

    //The repositories
    single { UsersRepository(get(named("apiStackUnderFlow"))) }

    //The view models
    single { UserViewModel(get()) }
    //single { ReleaseGroupsViewModel(get()) }
    //single { RecordingsViewModel(get(),get()) }

    // The API MusicBrain service will provide us with the data we need to display the artists, albums, and songs
    single(named("apiStackUnderFlow")) {
        createWebService<StackUnderFlowApiService>(
            get(
                named(Constants.apiStackUnderFlow)
            )
        )
    }

}