import request from '/@/utils/request';
enum Api {
  AddBase_Statistics = '/api/base_Statistics/add',
  DeleteBase_Statistics = '/api/base_Statistics/delete',
  UpdateBase_Statistics = '/api/base_Statistics/update',
  PageBase_Statistics = '/api/base_Statistics/page',
  GetDate_Statistics='/api/base_Statistics/dateList',
}

// 增加数据统计信息
export const addBase_Statistics = (params?: any) =>
	request({
		url: Api.AddBase_Statistics,
		method: 'post',
		data: params,
	});

// 删除数据统计信息
export const deleteBase_Statistics = (params?: any) => 
	request({
			url: Api.DeleteBase_Statistics,
			method: 'post',
			data: params,
		});

// 编辑数据统计信息
export const updateBase_Statistics = (params?: any) => 
	request({
			url: Api.UpdateBase_Statistics,
			method: 'post',
			data: params,
		});

// 分页查询数据统计信息
export const pageBase_Statistics = (params?: any) => 
	request({
			url: Api.PageBase_Statistics,
			method: 'post',
			data: params,
		});

//查询近两天统计数据
export const getDate_Statistics = (params?: any) => 
	request({
			url: Api.GetDate_Statistics,
			method: 'get',
			data: params,
		});


