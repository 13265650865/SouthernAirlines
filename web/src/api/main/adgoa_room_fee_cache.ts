import request from '/@/utils/request';
enum Api {
  Addadgoa_room_fee_cache = '/api/adgoa_room_fee_cache/add',
  Deleteadgoa_room_fee_cache = '/api/adgoa_room_fee_cache/delete',
  Updateadgoa_room_fee_cache = '/api/adgoa_room_fee_cache/update',
  Pageadgoa_room_fee_cache = '/api/adgoa_room_fee_cache/page',
  GetOption='/api/adgoa_room_fee_cache/getOption',
}

// 增加agdoa数据
export const addadgoa_room_fee_cache = (params?: any) =>
	request({
		url: Api.Addadgoa_room_fee_cache,
		method: 'post',
		data: params,
	});

// 删除agdoa数据
export const deleteadgoa_room_fee_cache = (params?: any) => 
	request({
			url: Api.Deleteadgoa_room_fee_cache,
			method: 'post',
			data: params,
		});

// 编辑agdoa数据
export const updateadgoa_room_fee_cache = (params?: any) => 
	request({
			url: Api.Updateadgoa_room_fee_cache,
			method: 'post',
			data: params,
		});

// 分页查询agdoa数据
export const pageadgoa_room_fee_cache = (params?: any) => 
	request({
			url: Api.Pageadgoa_room_fee_cache,
			method: 'post',
			data: params,
		});

export const GetOption = (params?: any) => 
		request({
			url: Api.GetOption,
			method: 'get',
			data: params,
		});


