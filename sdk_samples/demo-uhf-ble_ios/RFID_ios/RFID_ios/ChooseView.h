//
//  ChooseView.h
//  RFID_ios
//
//  Created by   on 2018/4/27.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ChooseView : UIView<UITableViewDelegate,UITableViewDataSource>

@property (nonatomic,strong)UITableView *tableView;
@property (nonatomic,strong)NSMutableArray *dataSource;


-(void)refreshTableViewWith:(NSMutableArray *)dataSource;


@property (nonatomic,copy)void (^removeBlock)(void);

@property(nonatomic,copy)void (^returnBlock)(NSInteger index);

@end
