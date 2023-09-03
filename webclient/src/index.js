import React, { createContext } from 'react';
import ReactDOM from 'react-dom/client';
import AppRouter from './Components/AppRouter/AppRouter';
import UserStore from 'Store/UserStore';

const root = ReactDOM.createRoot(document.getElementById('root'));
export const Context = createContext(null)

root.render(
	<React.StrictMode>
		<Context.Provider value={{
			user: new UserStore(),
		}}>
			<AppRouter />
		</Context.Provider>
	</React.StrictMode>
);