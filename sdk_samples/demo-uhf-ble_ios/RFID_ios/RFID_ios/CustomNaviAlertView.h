//
//  CustomNaviAlertView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

#import "BLEModel.h"

typedef void(^didSelectedItem)(BLEModel *model);

@interface CustomNaviAlertView : UIView

@property(nonatomic,strong) UITableView *myTableView;

@property(nonatomic,strong) UIButton *cannelBtn;
@property(nonatomic,strong) UIButton *againlBtn;

@property (nonatomic,copy)void (^removeBlock)(void);
@property (nonatomic,copy)void (^cancelBlock)(void);


@property (nonatomic, strong) NSMutableArray *dataSource;

@property (nonatomic, strong)didSelectedItem selectItem;

- (void)didSelectItem:(didSelectedItem)selectItem;

@end
