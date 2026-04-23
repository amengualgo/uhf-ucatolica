//
//  ScanTwoView.h
//  RFID_ios
//
//  Created by   on 2019/6/17.
//  Copyright © 2019年  . All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BSprogreUtil.h"
#import "RFIDBlutoothManager.h"

NS_ASSUME_NONNULL_BEGIN

@interface ScanView : UIView<UITableViewDelegate,UITableViewDataSource>


@property (nonatomic,strong)UITextField *ptrField;
@property (nonatomic,strong)UITextField *lenField;
@property (nonatomic,strong)UITextField *dataField;
@property (nonatomic,strong)UIButton *setFilterBtn;
@property (nonatomic,copy)void (^setFilterBlock)(void);

@property (nonatomic,strong)UIButton *singleBtn;
@property (nonatomic,strong)UIButton *beginBtn;
@property (nonatomic,strong)UIButton *stopBtn;
@property (nonatomic,strong)UIButton *cleanBtn;

@property (nonatomic,copy)void (^singleBlock)(void);
@property (nonatomic,copy)void (^beginBlock)(void);
@property (nonatomic,copy)void (^stopBlock)(void);
@property (nonatomic,copy)void (^cleanBlock)(void);

@property (nonatomic,strong)UILabel *tagLab;
@property (nonatomic,strong)UILabel *countLab;
@property (nonatomic,strong)UILabel *allCountLab;
@property (nonatomic,strong)UILabel *countLabString;
@property (nonatomic,strong)UITableView *tableView;

@property (nonatomic,assign)BANK filterBank;  // scan filter type
@property (nonatomic,strong)NSMutableArray *dataSource;
@property (nonatomic,assign)NSInteger allCount;

-(void) disConnectState;
-(void) connectState;

@end

NS_ASSUME_NONNULL_END
