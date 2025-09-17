import request from '/@/utils/request';
enum Api {
  AddBase_Searchlog = '/api/base_Searchlog/add',
  DeleteBase_Searchlog = '/api/base_Searchlog/delete',
  UpdateBase_Searchlog = '/api/base_Searchlog/update',
  PageBase_Searchlog = '/api/base_Searchlog/page',
}

// 增加爬虫数据统计
export const addBase_Searchlog = (params?: any) =>
	request({
		url: Api.AddBase_Searchlog,
		method: 'post',
		data: params,
	});

// 删除爬虫数据统计
export const deleteBase_Searchlog = (params?: any) => 
	request({
			url: Api.DeleteBase_Searchlog,
			method: 'post',
			data: params,
		});

// 编辑爬虫数据统计
export const updateBase_Searchlog = (params?: any) => 
	request({
			url: Api.UpdateBase_Searchlog,
			method: 'post',
			data: params,
		});

// 分页查询爬虫数据统计
export const pageBase_Searchlog = (params?: any) => 
	request({
			url: Api.PageBase_Searchlog,
			method: 'post',
			data: params,
		});


