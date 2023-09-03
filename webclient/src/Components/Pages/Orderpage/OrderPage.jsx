import Header from "Components/Header/Header"
import MenuList from "Components/MenuList/MenuList"
import OrderFooter from "Components/OrderFooter/OrderFooter"

const OrderPage = () => {
  return <>
    <Header
      title={"Заказ"}
      imagePath={"./img/header-order.jpg"}
      isNotLines={true} />
    <MenuList isOrder={true} />
    <OrderFooter />
  </>
}

export default OrderPage