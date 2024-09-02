/* prettier-ignore-start */

/* eslint-disable */

// @ts-nocheck

// noinspection JSUnusedGlobalSymbols

// This file is auto-generated by TanStack Router

// Import Routes

import { Route as rootRoute } from './routes/__root'
import { Route as RepositorioImport } from './routes/repositorio'
import { Route as IndexImport } from './routes/index'
import { Route as ProyectosIndexImport } from './routes/proyectos/index'
import { Route as ProyectosProyectoIdImport } from './routes/proyectos/$proyectoId'

// Create/Update Routes

const RepositorioRoute = RepositorioImport.update({
  path: '/repositorio',
  getParentRoute: () => rootRoute,
} as any)

const IndexRoute = IndexImport.update({
  path: '/',
  getParentRoute: () => rootRoute,
} as any)

const ProyectosIndexRoute = ProyectosIndexImport.update({
  path: '/proyectos/',
  getParentRoute: () => rootRoute,
} as any)

const ProyectosProyectoIdRoute = ProyectosProyectoIdImport.update({
  path: '/proyectos/$proyectoId',
  getParentRoute: () => rootRoute,
} as any)

// Populate the FileRoutesByPath interface

declare module '@tanstack/react-router' {
  interface FileRoutesByPath {
    '/': {
      preLoaderRoute: typeof IndexImport
      parentRoute: typeof rootRoute
    }
    '/repositorio': {
      preLoaderRoute: typeof RepositorioImport
      parentRoute: typeof rootRoute
    }
    '/proyectos/$proyectoId': {
      preLoaderRoute: typeof ProyectosProyectoIdImport
      parentRoute: typeof rootRoute
    }
    '/proyectos/': {
      preLoaderRoute: typeof ProyectosIndexImport
      parentRoute: typeof rootRoute
    }
  }
}

// Create and export the route tree

export const routeTree = rootRoute.addChildren([
  IndexRoute,
  RepositorioRoute,
  ProyectosProyectoIdRoute,
  ProyectosIndexRoute,
])

/* prettier-ignore-end */