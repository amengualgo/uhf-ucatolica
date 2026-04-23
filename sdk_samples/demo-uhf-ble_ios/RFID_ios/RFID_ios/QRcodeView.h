//
//  QRcodeView.h
//  RFID_ios
//
//  Created by hjl on 2018/10/19.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>
#import "RFIDBlutoothManager.h"

@interface QRcodeView : UIView<UITableViewDelegate,UITableViewDataSource>


@property (nonatomic,strong)UIButton *scanBtn;
@property (nonatomic,strong)UIButton *cleanBtn;
@property (nonatomic,strong)UIButton *continuousScanBtn;
@property (nonatomic,strong)UIButton *continuousStopBtn;
/** typeSelectBtn */
@property (nonatomic, strong) UIButton *typeSelectedBtn;

@property (nonatomic,copy)void (^scanBlock)(void);
@property (nonatomic,copy)void (^cleanBlock)(void);
@property (nonatomic,copy)void (^continuousScanBlock)(void);
@property (nonatomic,copy)void (^continuousStopBlock)(void);

@property (nonatomic,strong)UITableView *tableView;

@property (nonatomic,strong)NSMutableArray *dataSource;
/** selectedType */
@property (assign,nonatomic) BarcodeParsingType barcodeParsingType;

@end
