import request from '/@/utils/request';
enum Api {
  AddBase_Hotel = '/api/base_Hotel/add',
  DeleteBase_Hotel = '/api/base_Hotel/delete',
  UpdateBase_Hotel = '/api/base_Hotel/update',
  PageBase_Hotel = '/api/base_Hotel/page',
  Detail_Hotel = '/api/base_Hotel/detail',
  GetHotDetail = '/api/base_Hotel/GetHotDetail',
}

// 增加酒店列表
export const addBase_Hotel = (params?: any) =>
	request({
		url: Api.AddBase_Hotel,
		method: 'post',
		data: params,
	});

// 删除酒店列表
export const deleteBase_Hotel = (params?: any) => 
	request({
			url: Api.DeleteBase_Hotel,
			method: 'post',
			data: params,
		});

// 编辑酒店列表
export const updateBase_Hotel = (params?: any) => 
	request({
			url: Api.UpdateBase_Hotel,
			method: 'post',
			data: params,
		});

// 分页查询酒店列表
export const pageBase_Hotel = (params?: any) => 
	request({
			url: Api.PageBase_Hotel,
			method: 'post',
			data: params,
		});

// 酒店房型列表
export const detail_Hotel = (params?: any) => 
	request({
			url: Api.Detail_Hotel,
			method: 'get',
			data: params,
		});
// 获取携程酒店房型实时信息
export const getHotDetail = (params?: any) => 
	request({
			url: Api.GetHotDetail,
			method: 'post',
			data: params,
		});
		

