import CategoryList from "Components/CategoryList/CategoryList";
import { Container, Spinner } from 'react-bootstrap';
import { useEffect, useState } from "react";
import { getMenu } from "Api/ApiRequest";

const MenuList = () => {
	const [menu, setMenu] = useState([])
	const [isLoad, setIsLoad] = useState(false)

	function group(data) {
		let groupMenu = {}
		data.map(el => {
			if (!Array.isArray(groupMenu[el.categoryName]))
				groupMenu[el.categoryName] = [];
			groupMenu[el.categoryName].push(el)
		})
		return groupMenu
	}

	useEffect(() => {
		(async function () {
			await getMenu().then((res) => {
				setMenu(group(res.data))
			}).finally(() => setIsLoad(true))
		}())
	}, [])

	return <Container className="d-flex flex-column align-items-center mb-5">
		{isLoad ? menu && Object.entries(menu).map((item) => {
			return <CategoryList key={JSON.stringify(item)} title={item[0]} items={item[1]} />
		}) : <Spinner className="m-5" />}
	</Container>
}

export default MenuList