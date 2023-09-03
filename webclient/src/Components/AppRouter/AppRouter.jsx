import "./AppRouter.css"
import MainPage from "Components/Pages/MainPage/MainPage";
import MenuPage from "Components/Pages/MenuPage/MenuPage";
import LogInPage from "Components/Pages/LogInPage/LogInPage";
import RegistrationPage from "Components/Pages/RegistrationPage/RegistrationPage";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useContext, useEffect } from "react";
import { Context } from "index";
import { observer } from "mobx-react-lite";
import ReservationPage from "Components/Pages/ReservationPage/ReservationPage";
import OrderPage from "Components/Pages/Orderpage/OrderPage";
import ConfirmOrderPage from "Components/Pages/ConfirmOrderPage/ConfirmOrderPage";
import { checkToken } from "Api/ApiRequest"

const AppRouter = observer(() => {
	const { user } = useContext(Context);
	useEffect(() => {
		(async function () {
			let isValidToken = await checkToken()
			if (!isValidToken)
				user.setEmail('');

			user.setIsAuth(isValidToken);
		})();
	}, [user]);

	return <>
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<MainPage />} />
				<Route path="menu" element={<MenuPage />} />
				<Route path="login" element={<LogInPage />} />
				<Route path="reservation" element={<ReservationPage />} />
				<Route path="order" element={<OrderPage />} />
				<Route path="confirm-order" element={<ConfirmOrderPage />} />
				<Route path="registration" element={<RegistrationPage />} />
			</Routes>
		</BrowserRouter>
	</>
})

export default AppRouter;