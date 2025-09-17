import request from '/@/utils/request';
enum Api {
  AddBase_Referer = '/api/base_Referer/add',
  DeleteBase_Referer = '/api/base_Referer/delete',
  UpdateBase_Referer = '/api/base_Referer/update',
  PageBase_Referer = '/api/base_Referer/page',
  GetOption_Referer='/api/base_Referer/getOption',
  List_Referer='/api/base_Referer/list',
  Hotel_detail='/api/base_Referer/hoteldetail',
}

// 增加访问数据明细
export const addBase_Referer = (params?: any) =>
	request({
		url: Api.AddBase_Referer,
		method: 'post',
		data: params,
	});

// 删除访问数据明细
export const deleteBase_Referer = (params?: any) => 
	request({
			url: Api.DeleteBase_Referer,
			method: 'post',
			data: params,
		});

// 编辑访问数据明细
export const updateBase_Referer = (params?: any) => 
	request({
			url: Api.UpdateBase_Referer,
			method: 'post',
			data: params,
		});

// 分页查询访问数据明细
export const pageBase_Referer = (params?: any) => 
	request({
			url: Api.PageBase_Referer,
			method: 'post',
			data: params,
});
export const getOption_Referer = (params?: any) => 
request({
	url: Api.GetOption_Referer,
	method: 'get',
	data: params,
});
export const list_Referer = (params?: any) => 
request({
	url: Api.List_Referer,
	method: 'get',
	data: params,
});
export const hotel_detail = (params?: any) => 
request({
	url: Api.Hotel_detail,
	method: 'post',
	data: params,
});





