import { useNavigate } from "@tanstack/react-router";
import { Article } from "@/types";

interface ArticleCardProps {
  article: Article;
}

function ArticleCard({ article }: ArticleCardProps) {
  const router = useNavigate();

  return (
    <div className="text-white rounded-xl p-6 mb-4 shadow-lg" style={{backgroundColor: "#262626"}}>
      <div className="flex items-center mb-2">
        <span className="bg-green-500 text-xs font-semibold text-white px-2 py-1 rounded">
          {article.tag}
        </span>
      </div>
      <h2 className="text-xl font-bold mb-2">{article.name}</h2>
      <div className="text-gray-400 text-sm mb-4">
        <span style={{color: "#acb7a3"}}>{article.author}</span>
        <span className="mx-2">•</span>
        <span>{article.date}</span>
      </div>
      <p className="text-gray-300 mb-4">
        {article.desc_short}
      </p>
      <div className="flex justify-between items-center">
        <button
          onClick={() => router({ to: `/noticias/${article.id}` })}
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        >
          Ver ahora
        </button>
        <div className="flex items-center text-gray-400">
          <span className="mr-2">{/* Aquí iría el número de likes, si lo tienes */}</span>
          <button className="mr-4">
            <i className="fa fa-thumbs-up" />
          </button>
          <button>
            <i className="fa fa-thumbs-down" />
          </button>
        </div>
      </div>
    </div>
  );
}

export default ArticleCard;
