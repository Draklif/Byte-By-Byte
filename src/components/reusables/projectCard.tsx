import { Link } from "@tanstack/react-router";
import { Project } from "@/types";

interface ProjectCardProps {
  project: Project;
}

function ProjectCard({ project }: ProjectCardProps) {
  return (
    <div className="flex bg-gray-800 text-white p-4 rounded-lg shadow-lg w-full items-stretch">
      <div className="flex-none w-1/4">
        <img src={project.images[0]} alt={project.name} className="rounded-lg w-full h-auto object-cover" />
      </div>
      <div className="flex-grow px-4 py-2">
        <h3 className="text-lg font-semibold">{project.name}</h3>
        <div className="flex flex-wrap gap-1 my-2 mb-4">
          {project.tags.map(tag => (
            <span key={tag} className="bg-gray-700 text-sm rounded-full px-2 py-1">
              {tag}
            </span>
          ))}
        </div>
        <p className="text-sm">{project.date}</p>
        <div className="mt-auto">
          <p className="text-xs">
            {project.authors.map(author => author.name).join(", ")}
          </p>
        </div>
      </div>
      <div className="flex flex-col justify-end w-1/5 p-2">
        <Link to={`/proyectos/${project.id}`} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded self-end mt-auto">
          Ver ahora
        </Link>
      </div>
    </div>
  );
}

export default ProjectCard;
