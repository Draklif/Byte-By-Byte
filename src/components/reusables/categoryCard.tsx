function CategoryCard({ category, backgroundImage, onClick, isActive }) {
  return (
    <div
      className={`relative w-48 h-48 rounded-lg overflow-hidden shadow-lg cursor-pointer ${isActive ? 'ring-2 ring-blue-500' : ''}`}
      onClick={onClick}
    >
      <img
        src={backgroundImage}
        alt={category}
        className="absolute inset-0 w-full h-full object-cover filter blur-sm"
      />
      <div className="absolute inset-0 bg-black bg-opacity-25 flex items-center justify-center">
        <span className="text-white text-xl font-semibold">{category}</span>
      </div>
    </div>
  );
}

export default CategoryCard;
