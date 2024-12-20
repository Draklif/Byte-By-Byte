import { createRootRoute, Outlet } from '@tanstack/react-router'

export const Route = createRootRoute({
  component: () => (
    <>
      {/* <div className="p-2 flex gap-2">
        <Link to="/" className="[&.active]:font-bold">
          Home
        </Link>{' '}
        <Link to="/proyectos" className="[&.active]:font-bold">
          Proyectos
        </Link>
        <Link to="/repositorio" className="[&.active]:font-bold">
          Repositorio
        </Link>
      </div> */}
      <Outlet />
    </>
  ),
})
