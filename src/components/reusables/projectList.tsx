import ProjectCard from "@/components/reusables/projectCard";

function ProjectList({ projects, numPages, currentPage, setCurrentPage }) {
  const getPageButtons = () => {
    const pageButtons = [];
    const visiblePages: number[] = [];

    let start = Math.max(2, currentPage - 1);
    let end = Math.min(numPages - 1, currentPage + 1);

    if (currentPage < 4) {
      end = Math.min(5, numPages - 1);
      visiblePages.push(1);
    }
    if (currentPage > numPages - 4) {
      start = Math.max(numPages - 4, 2);
    }

    if (start > 2) visiblePages.push(1);
    for (let i = start; i <= end; i++) {
      if (!visiblePages.includes(i)) {
        visiblePages.push(i);
      }
    }
    if (numPages > 1 && !visiblePages.includes(numPages)) {
      visiblePages.push(numPages);
    }

    let previous = 0;
    for (let i = 0; i < visiblePages.length; i++) {
      if (visiblePages[i] !== previous + 1 && previous !== 0) {
        pageButtons.push(
          <span key={"ellipsis-" + i} className="px-3 py-1 text-white">
            ...
          </span>
        );
      }
      pageButtons.push(
        <button
          key={visiblePages[i]}
          onClick={() => setCurrentPage(visiblePages[i])}
          className={`px-3 py-1 rounded ${currentPage === visiblePages[i] ? "bg-blue-500" : "bg-gray-700"} text-white`}
        >
          {visiblePages[i]}
        </button>
      );
      previous = visiblePages[i];
    }

    return pageButtons;
  };

  return (
    <div className="flex-grow">
      <div className="flex flex-col items-center gap-4">
        {projects.map((project, index) => (
          <ProjectCard key={index} project={project} />
        ))}
      </div>
      {numPages > 1 && (
        <div className="flex justify-end items-center gap-3 mt-4">
          <button
            onClick={() =>
              setCurrentPage((current) => Math.max(current - 1, 1))
            }
            disabled={currentPage === 1}
            className="px-3 py-1 rounded bg-gray-700 text-white"
          >
            Anterior
          </button>
          {getPageButtons()}
          <button
            onClick={() =>
              setCurrentPage((current) => Math.min(current + 1, numPages))
            }
            disabled={currentPage === numPages}
            className="px-3 py-1 rounded bg-gray-700 text-white"
          >
            Siguiente
          </button>
        </div>
      )}
    </div>
  );
}

export default ProjectList;
