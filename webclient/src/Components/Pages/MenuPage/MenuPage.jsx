import Header from "Components/Header/Header";
import MenuList from "Components/MenuList/MenuList";

function MenuPage() {
	return <>
		<Header
			title={"Меню"}
			imagePath={"./img/header-order.jpg"}
			isNotLines={true} />
		<MenuList />
	</>
}

export default MenuPage;