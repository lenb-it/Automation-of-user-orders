import Header from "Components/Header/Header";
import SectionBody from "Components/SectionBody/SectionBody";
import SectionHeader from "Components/SectionHeader/SectionHeader";

const secondImages = ["./img/second_1.jpg", "./img/second_2.jpg", "./img/second_3.jpg"]
const thirdImages = ["./img/third_1.jpg", "./img/third_2.jpg", "./img/third_3.jpg"]

function MainPage() {
	return <>
		<Header title="Клевер" subtitle="Всё для вас" imagePath={"./img/mainpage.jpg"} />
		<main className="">
			<SectionBody
				title={"О нас"}
				subtitle={"Желаете окунуться в атмосферу белорусского быта, насладиться вкусно приготовленной едой по старинным рецептам и душевно провести вечер? Ресторан «Беларуская хата» ждет вас в уютном зале с атмосферой настоящей белорусской глубинки! Отличительная особенность ресторана – это мини-музей с национальными предметами старины. Уже сейчас вы можете прикоснуться к раритетной магнитоле, диковинному утюгу или граммофону."}
				imagePath={"./img/about_us.jpg"} />
			<SectionHeader
				title={"Рецепты"}
				imagePath={"./img/recept.jpg"} />
			<SectionBody
				title={"Меню"}
				subtitle={"Вас приятно удивит авторская кухня, где вы найдете оригинальное исполнение полюбившихся «бабушкиных» рецептов, а музей самобытной культуры и необычная стилистика сделают вечер поистине особенным!"}
				imagePathes={secondImages} />
			<SectionHeader
				title={"Смесь"}
				imagePath={"./img/blend.jpg"} />
			<SectionBody
				title={"Восторг"}
				subtitle={"Мы обещаем приятное времяпрепровождение, которое предложит что-то необычное местным и иностранным посетителям и гарантирует, что вы каждый раз будете наслаждаться незабываемыми блюдами."}
				imagePathes={thirdImages} />
		</main>
	</>
}

export default MainPage;