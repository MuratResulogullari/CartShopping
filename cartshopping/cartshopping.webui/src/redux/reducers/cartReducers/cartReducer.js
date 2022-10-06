import * as actionTypes from '../../actions/actionTypes';
import initialState from "../initialState";

export default function cartReducer(state = initialState.cartItems, action) {
    switch (action.type) {
        case actionTypes.GET_CARTITEMS_SUCCESS:
            return action.payload;
        default:
            return state;
    }
}