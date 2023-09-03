import "./CategoryList.scss"
import CategoryItem from "Components/CategoryItem/CategoryItem"
import { useLocation } from "react-router-dom"

const CategoryList = ({ title, items }) => {
	const location = useLocation()

	return <div className={`category-list ${location.pathname === '/order' && 'category-list-order'} `}>
		<h3 className="category-list__title fs-2">{title}</h3>
		<div className="category-list-items">
			{items && items.map(item => <CategoryItem key={JSON.stringify(item)} pathname={location.pathname} item={item} />)}
		</div>
	</div>
}

export default CategoryList