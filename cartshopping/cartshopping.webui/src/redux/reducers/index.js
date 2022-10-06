import { combineReducers } from "redux";
import cartReducer from './cartReducers/cartReducer';
const rootReducer = combineReducers({
    cartReducer
});

export default rootReducer;