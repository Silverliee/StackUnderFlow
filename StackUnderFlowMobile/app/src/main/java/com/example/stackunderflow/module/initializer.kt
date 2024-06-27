package com.example.stackunderflow.module

import android.content.Context
import org.koin.android.ext.koin.androidContext
import org.koin.core.context.loadKoinModules
import org.koin.core.context.startKoin
import org.koin.core.error.ApplicationAlreadyStartedException

// that function inject module dependencies using Koin
fun injectModuleDependencies(context: Context) {
    try {
        //Start Koin and load modules
        startKoin {
            androidContext(context)
            modules(modules)
        }
    } catch (alreadyStart: ApplicationAlreadyStartedException) {
        //If Koin is already started, load the modules
        loadKoinModules(modules)
    }
}

// List of modules to be loaded
private val modules = mutableListOf(coreModules, remoteModule)