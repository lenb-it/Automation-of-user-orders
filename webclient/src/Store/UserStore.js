import { makeAutoObservable } from "mobx"

class UserStore {
	constructor() {
		this._isAuth = false;
		this._email = ''
		this._password = ''
		this._reginFields = {}
		this._orders = []
		makeAutoObservable(this);
	}

	setIsAuth(value) {
		this._isAuth = value;
	}

	setReginFields(key, val) {
		this._reginFields[key] = val;
	}

	setUserAuth(key, val) {
		this._userAuth[key] = val;
	}

	setEmail(val) {
		this._email = val;
	}

	setPassword(val) {
		this._password = val;
	}

	setOrders(order) {
		let orderCurr = this._orders.find(el => el.id === order.id)
		if (orderCurr)
			orderCurr.count = order.count
		else
			this._orders.push({
				name: order.name,
				price: order.price,
				count: order.count,
				id: order.id
			})
		this._orders = this._orders.filter(order => order.count > 0)
	}

	get orders() {
		return this._orders
	}

	get email() {
		return this._email
	}

	get password() {
		return this._password
	}

	get isAuth() {
		return this._isAuth;
	}

	get reginFields() {
		return this._reginFields;
	}
}

export default UserStore;