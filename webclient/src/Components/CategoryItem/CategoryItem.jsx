import { Context } from "index"
import "./CategoryItem.scss"
import { useContext, useState, useEffect } from "react"
import { Button } from "react-bootstrap"
import { observer } from "mobx-react-lite"

const CategoryItem = ({ item, pathname }) => {
	const { user } = useContext(Context)
	const [count, setCount] = useState(0)

	function addToOrder() {
		setCount(prev => prev + 1)
		user.setOrders({ name: item.name, price: item.price, count: count + 1, id: item.id })
	}

	function removeFromOrder() {
		setCount(prev => count > 0 ? prev - 1 : 0)
		user.setOrders({ name: item.name, price: item.price, count: count - 1 })
	}

	useEffect(() => {
		let order = user.orders.find(el => el.name === item.name)
		if (order)
			setCount(order.count)
	}, [item.name, user.orders])

	return <div className={`category-item ${pathname === '/order' && 'category-item-order'} ${item.discount && "discount"}`}>
		<div className="item-title">
			<p className="item-title__name">{item.name}</p>
			{pathname !== '/order' && <p className="item-title__price">{item.price} р.</p>}
		</div>
		{pathname === '/order' ? <div className="item-add">
			<p className="item-title__price">{item.price} р.</p>
			<p className="item-title__count">{count}</p>
			<Button
				variant="dark"
				onClick={removeFromOrder}>-</Button>
			<Button
				variant="dark"
				onClick={addToOrder}>+</Button>
		</div> : <p className="category-item__desc">{item.description}</p>}
	</div>
}

export default observer(CategoryItem)